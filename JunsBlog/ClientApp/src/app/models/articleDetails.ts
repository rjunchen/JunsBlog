import { User } from './user';
import { Article } from './article';

export class ArticleDetails extends Article{
    didILike: boolean;
    likesCount: number;
    didIDislike: boolean;
    didIFavored: boolean;
    author: User;
    lastModifiedDate: string;
    views: number;
    content: string;
    _id: string;
    comments: Array<string>;
}
