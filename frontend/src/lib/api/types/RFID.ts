export type RFID = {
    id: string;
    name: string;
    image?: string;
}
export type RFIDTrack = {
    type: "Track";
    artists: string[]
} & RFID;

export type RFIDAlbum = {
    type: "Album";
    artists: string[];
} & RFID;

export type RFIDPlaylist = {
    type: "Playlist";
    description?: string;
} & RFID;

export type RFIDArtist = {
    type: "Artist";
} & RFID;