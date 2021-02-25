use serde::{Serialize, Deserialize};
use bson::document::Document;
use mongodb::bson::doc;



#[derive(Deserialize, Serialize, Clone)]
pub struct Post{
    pub title: String,
    pub content: String,
    pub user: String,
    pub categorie: String,
}

impl Post{
    pub fn new_empty() -> Post{
        Post{
            title: "null".to_string(),
            content: "null".to_string(),
            user: "null".to_string(),
            categorie: "null".to_string(),
        }
    }

    pub fn to_doc(self) -> Document{
        doc!{
            "title": self.title,
            "content": self.content,
            "user": self.user,
            "categorie": self.categorie,
        }
    }
}

#[derive(Deserialize, Serialize)]
pub struct PostResponse{
    pub result: String,
    pub post: Post,
}

