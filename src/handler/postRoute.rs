use serde_json::json;
use rocket::response::content;
use crate::mongo_connection::Conn;
use rocket::http::{CookieJar, Cookie};

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
