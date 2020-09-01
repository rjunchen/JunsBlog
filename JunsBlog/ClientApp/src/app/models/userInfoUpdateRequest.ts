export class UserInfoUpdateRequest {
    image: string;
    name: string;
    id: string;
    email: string;
    constructor(id: string, name: string, email: string, image: string){
        this.id = id;
        this.name = name;
        this.email = email;
        this.image = image;
    }
}
