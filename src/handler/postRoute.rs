use serde_json::json;
use rocket::response::content;
use rocket::http::{CookieJar, Cookie};
use rocket_contrib::json::Json;

use crate::mongo_connection::Conn;
use crate::model::post::{Post, PostResponse};
use crate::handler::check_user_id_cookie;

#[get("/")]
pub fn index(_conn: Conn) -> String{
    "helljjo world".to_string()
}


#[get("/allPost")]
pub fn get_all_posts(conn: Conn) -> content::Json<String>{
    // we get the collection with the connection
    let collection = conn.get_post_collection();
    // next we get the cursor on all post
    let cursor = collection.find(None, None).unwrap();

    let mut posts = Vec::new();

    //we push to the vec every document after converted it in String
    for post in cursor{
       posts.push(post.unwrap().to_string()); 
    }

    // we create a json withe serde_json with the vec
    let json_result = json!(posts);
    
    content::Json(json_result.to_string())
}



#[post("/newPost", data = "<new_post>")]
pub fn new_post(cookies: &CookieJar<'_>, conn: Conn, new_post: Json<Post>) -> Json<PostResponse>{
    let post_collection = conn.get_post_collection();

    // check if the user_id id the same in the private and public cookie
    // if not return directly
    if !check_user_id_cookie(cookies){
        return Json(PostResponse{
            result: "no connection".to_string(),
            post: Post::new_empty(),
        });
    }

    //TODO: check also the username

    //TODO: impl an to doc in Post struct
    new_post.into_inner();

    Json(PostResponse{
        result: "test".to_string(),
        post: Post{
            title: "test".to_string(),
            content: "test".to_string(),
            user: "celeste".to_string(),
            categorie: "test".to_string(),
        }
    })
}
