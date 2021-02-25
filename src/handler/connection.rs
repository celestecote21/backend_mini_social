use crate::mongo_connection::Conn;
use crate::user::User;
use rocket::response::content;
use rocket::request::{self, Form, FlashMessage, FromRequest, Request};
use rocket::response::{Redirect, Flash};
use rocket::http::{Cookie, CookieJar};
use sha3::{Digest, Sha3_256};
use mongodb::bson::doc;
use hex::{encode, decode};
use serde::{Serialize, Deserialize};
use rocket_contrib::json::Json;

#[derive(Deserialize)]
pub struct Login {
    user_name: String,
    password: String
}

#[derive(Deserialize)]
pub struct CookieLogin {
    user_name: String,
    cookie_saved: String,
}

#[derive(Deserialize)]
pub struct SignUp {
    user_name: String,
    password: String,
    email: String,
    first_name: String,
    last_name: String,
}

#[derive(Deserialize, Serialize)]
pub struct LoginResponse {
    result: String,
    user: User,
}


 

#[post("/signUp", data = "<sign_up>")]
pub fn sign_up(cookies: &CookieJar<'_>, conn: Conn, sign_up: Json<SignUp>) -> Json<LoginResponse>{
    let users_collection = conn.get_users_collection();

    // first we test if the user is not already in the database
    let user_test = match users_collection.find_one(doc!{"user_name": &sign_up.user_name[..]}, None){
        Ok(user) => user,
        _ => {
            println!("no database");
            return Json(LoginResponse{
                result: "no database".to_string(),
                user: User::new_empty(),
            });
        }
    };
    let final_doc;
    match user_test{
        Some(_) => {
            return Json(LoginResponse{
                result: "bad_username".to_string(),
                user: User::new_empty(),
            });
        },
        None => {
            let mut hasher = Sha3_256::new();
            hasher.update(&sign_up.password[..]);
            let password_sha = encode(hasher.finalize());
           final_doc = doc!{
                "email": &sign_up.email[..],
                "user_name": &sign_up.user_name[..],
                "first_name": &sign_up.first_name[..],
                "last_name": &sign_up.last_name[..],
                "password": &password_sha[..],
                "user_id": 1.to_string(),
                "profil_picture_url": "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fthumb%2F7%2F7e%2FCircle-icons-profile.svg%2F1024px-Circle-icons-profile.svg.png&f=1&nofb=1".to_string(),
            };
            match users_collection.insert_one(final_doc.clone(), None){
                Ok(_) => {}
                Err(err) => {
                    println!("{}", err);
                    return Json(LoginResponse{
                        result: "internal error".to_string(),
                        user: User::new_empty(),
                    });
                }
            };
        }
    }
    cookies.add_private(Cookie::new("user_id", 1.to_string()));
    Json(LoginResponse{
        result: "success".to_string(),
        user: User::from_database(final_doc)
    })

}


// route to login with the username and password
#[post("/login", data = "<login>")]
pub fn login(cookies: &CookieJar<'_>, conn: Conn, login: Json<Login>) -> Json<LoginResponse> {

    // find the document describing the user and unwrap it
    let user_doc = match conn.find_user_doc(&login.user_name[..]){
        Ok(user_d) => user_d,
        Err(e) => return Json(LoginResponse{
            result: e,
            user: User::new_empty(),
        }),
    };

    // get the sha of the password in the database
    let password_db = user_doc.get("password").unwrap().as_str().unwrap();
    // create the sha of the provided password
    let mut hasher = Sha3_256::new();
    hasher.update(&login.password[..]);
    let password_sha = encode(hasher.finalize());
    

    let user_doc_temp = user_doc.clone();
    // test if the password is the same
    if password_sha == password_db{
        println!("co");
        // add a cookie in the response, the cookie is the user id encrypeted with a 128bit
        // certificat
        cookies.add_private(Cookie::new("user_id", user_doc_temp.get_str("user_id").unwrap().to_owned()));
        cookies.add(Cookie::new("id", user_doc_temp.get_str("user_id").unwrap().to_owned()));
        Json(LoginResponse{
            result: "success".to_string(),
            user: User::from_database(user_doc),
        })
    } else {
        Json(LoginResponse{
            result: "bad username/password".to_string(),
            user: User::new_empty(),
        })
    }
}



// route to login with the private cookie and the user name

#[post("/login_cookie", data = "<login>")]
pub fn login_with_cookie(cookies: &CookieJar<'_>, conn: Conn, login: Json<CookieLogin>) -> Json<LoginResponse> {
    let user_collection = conn.get_users_collection();
    // get the cookie in the connection
    let cookie = match cookies.get_private("user_id"){
        Some(cookie) => cookie,
        None => return Json(LoginResponse{
                result: "no cookie".to_string(),
                user: User::new_empty(),
            }),
    };
    // find the document describing the user and unwrap it
    let user_doc = match user_collection.find_one(doc!{"user_name" : &login.user_name[..]}, None){
        Ok(user1) => user1, 
        Err(_) => {
            // TODO: redirect
            println!("no database");
            return Json(LoginResponse{
                result: "no database".to_string(),
                user: User::new_empty(),
            });
        }
    };
     let user_doc = match user_doc{
         Some(user) => user,
         _  =>  return Json(LoginResponse{
                result: "bad user".to_string(),
                user: User::new_empty(),
            }),
     }; 
     
    let user_id = user_doc.get("user_id").unwrap().as_str().unwrap();

    // test the user_id of the database with the user_id of the cookie
    if cookie.value() == user_id{
        // if it's all good
        Json(LoginResponse{
            result: "success".to_string(),
            user: User::from_database(user_doc)
        })
    }else{
        Json(LoginResponse{
            result: "bad cookie".to_string(),
            user: User::new_empty(),
        })
    }
}

