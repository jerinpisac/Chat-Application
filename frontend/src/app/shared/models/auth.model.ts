export class loginModel {
    email : string = "";
    password : string = "";
}

export class registerModel {
    fullName : string = "";
    email : string = "";
    password : string = "";
    bio : string = "";
    language : string = "";
    status : string = "";
    profilePic : string = "";
}

export class registerDetailsModel {
    fullName : string = "";
    email : string = "";
    password : string = "";
}

export class userModel {
    id : number = 0;
    fullName : string = "";
    email : string = "";
    bio : string = "";
    language : string = "";
    status : string = "";
    profilePic : string = "";
    joinedAt : string = "";
    sentRequest : boolean = false;
}