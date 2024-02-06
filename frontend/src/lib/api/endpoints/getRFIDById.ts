import { get } from '$lib/api/fetch';

type GetRFIDByIdResponse = Track | Album | Playlist | Artist;

type RFID = {
    rfid: string;
    name: string;
    image?: string;
}

type Track = {
    type: "Track";
    artists: string[]
} & RFID;

type Album = {
    type: "Album";
    artists: string[];
} & RFID;

type Playlist = {
    type: "Playlist";
    description?: string;
} & RFID;

type Artist = {
    type: "Artist";
} & RFID;

export async function getRFIDById(id: string) {
    return await get<GetRFIDByIdResponse>(`/rfids/${id}`);
}