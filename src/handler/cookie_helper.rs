use rocket::http::{CookieJar, Cookie};



pub fn check_user_id_cookie(cookies: &CookieJar<'_>) -> bool{
    // get the encrypted user_id in the private cookie
    let private_cookie = match cookies.get_private("user_id"){
        Some(cookie) => cookie,
        None => return false,
    };
    // get the user_id in the public id
    let public_cookie = match cookies.get("id"){
        Some(cookie) => cookie,
        None => return false,
    };

    // test if the two cookies have the same value
    if public_cookie.value() != private_cookie.value(){
        return false;
    }else{
        return true;
    }
}
