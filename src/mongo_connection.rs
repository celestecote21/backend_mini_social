use rocket::request::{self, FromRequest};
use rocket::{Request, State};
use rocket::outcome::Outcome;
use mongodb::sync::Client;
use mongodb::options::ClientOptions;
use mongodb::sync::Collection;



/// to connect to the database
pub async fn init_mongo_client() -> Client{
    let mut client_options = ClientOptions::parse("mongodb://127.0.0.1:27017").await.unwrap();
    client_options.app_name = Some(String::from("backend_mini_social"));
    Client::with_options(client_options).unwrap()
}



pub struct Conn(pub Client);

#[rocket::async_trait]
impl<'a, 'r> FromRequest<'a, 'r> for Conn {
    type Error = std::convert::Infallible;

    // for every reauest with a Conn as guard
    async fn from_request(request: &'a Request<'r>) -> request::Outcome<Conn, Self::Error> {
        //let conn = try_outcome!(request.guard::<&Conn>().await);
        let client = request.guard::<State<Client>>().await;
        let conn = Conn(client.unwrap().inner().clone());
        Outcome::Success(conn)
        
    }
}

// to our Conn struct we implement some helper to get the data easier
impl Conn{
    pub fn get_collections_name(self) -> Vec<String>{
       self.0.database("post").list_collection_names(None).unwrap()
    }

    pub fn get_post_collection(self) -> Collection{
        self.0.database("post").collection("post")
    }
    
    pub fn get_users_collection(self) -> Collection{
        self.0.database("post").collection("users")
    }
}

