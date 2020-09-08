export class Article {
    id: string;
    title: string;
    content: string;
    coverImage: string;
    abstract: string;
    isPrivate: boolean;
    categories: Array<string>;

    constructor(){
        this.categories = [];
        this.isPrivate = false;
    }
}