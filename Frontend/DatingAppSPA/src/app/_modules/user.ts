import { Photo } from './photo';

export interface User {
    id: number;
    name: string;
    knowAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    interess?:  string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
}
