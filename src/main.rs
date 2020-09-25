#![feature(decl_macro, proc_macro_hygiene)]
#[macro_use]
extern crate rocket;
extern crate mongodb;

use rocket::Rocket;

mod mongo_connection;
use mongo_connection::init_mongo_client;

mod user;

pub mod handler;
pub mod model;

#[launch]
async fn init_rocket() -> Rocket{
    Rocket::ignite().manage(init_mongo_client().await)
        .mount("/", routes![handler::postRoute::index, 
            handler::postRoute::get_all_posts, 
            handler::connection::login,
            handler::connection::login_with_cookie,
            handler::connection::sign_up
        ])
}

