# JunsBlog
Juns Blog is blog web application, it is built with .NetCore, Angular 10 and MongoDB.

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 10.0.7.

# Currently main features of the blog web application
- User can register accounts
- User can authenticate with registered accounts or with social media accounts
- User can reset password
- user can post articles and view articles.
- User can comment on articles.
- User can reply on comments.
- User can like or dislike article or comments
- User can favor or undo favor on articles
- User can view and update user profile
- User can post articles with pictures or videos as well as text. 
- User can search article with keyword
- Articles can display total view, total likes
- Article keeps track if a user have been liked, disliked and favored the article itself

# Future works
- Admin can manger Users and article
- Users can manager article
- Users can send private message to users
- User can follow and undo follow users

# Main node packages and technologies that used within the application
- Angular Material UI - application UI framework
- ngx-infinite-scroll - combine use with mongoose pagination library to allow contents to be loaded when needed for better performance
- ngx-quill - rich text editor 
- quill-image-resize-module - resizing image in the quill text editor
- ngx-gallery-9 - display images
- ngx-image-cropper - crop image for the avatar

# Requirements for fully running this application locally
- Visual studio 2019
- Node.js and mongoDB need to be installed on the computer 
- Create Google developer account to get the client_id and client_secret for google OAuth
- Setup and email account for email notification
- Fill in the settings in the appsetting.json

## Install required node packages
Run `npm install` for installing the required node packages

## Development RESTful API Server

Run the JunsBlog project in Visual for a dev RESTful API server to start.

## Development Client Application

Run `npm run dev` in the ClientApp folder for a dev client application. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

# Unit Test
The units test are partially implemented.
