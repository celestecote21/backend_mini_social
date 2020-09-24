extern crate bson;
use serde::{Serialize, Deserialize};
use bson::Document;

#[derive(Serialize, Deserialize)]
pub struct User {
    user_name: String,
    user_id: u64,
    name: String,
    profil_picture_url: String
}


impl User{
    pub fn from_database(user_doc: Document) -> User{
        User{
            user_name: user_doc.get_str("user_name").unwrap().to_string(),
            user_id: user_doc.get_str("user_id").unwrap().parse::<u64>().unwrap(),
            name: user_doc.get_str("first_name").unwrap().to_string(),
            profil_picture_url: user_doc.get_str("profil_picture_url").unwrap().to_string(),
            }
    }

    pub fn new_empty() -> User{
        User{
            user_name: "null".to_string(),
            user_id: 0,
            name: "null".to_string(),
            profil_picture_url: "null".to_string(),
        }
    }
}
