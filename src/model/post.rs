use serde::{Serialize, Deserialize};



#[derive(Deserialize, Serialize)]
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
}

#[derive(Deserialize, Serialize)]
pub struct PostResponse{
    pub result: String,
    pub post: Post,
}

