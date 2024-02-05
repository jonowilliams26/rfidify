import { get } from '$lib/api/fetch';

type GetRFIDsResponse = GetRFIDsResponseItem[];
type GetRFIDsResponseItem = Track | Album | Playlist | Artist;

type RFID = {
    rfid: string;
    name: string;
    image?: string;
}

export type Track = {
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

export async function getRFIDs() {
    return await get<GetRFIDsResponse>('/rfids');
}