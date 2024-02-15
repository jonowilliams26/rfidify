export type RFID = {
    id: string;
    spotifyItem: Track | Artist | Album | Playlist;
}

type Track = {
    type: "Track";
    id: string;
    name: string;
    uri: string;
    artists: Artist[];
    album: Album;
}

type Artist = {
    type: "Artist";
    id: string;
    name: string;
    uri: string;
    images: Image[];
}

type Album = {
    type: "Album";
    id: string;
    name: string;
    uri: string;
    images: Image[];
    artists: Artist[];
}

type Playlist = {
    type: "Playlist";
    id: string;
    name: string;
    uri: string;
    description: string;
    images: Image[];
}

type Image = {
    url: string;
    width?: number;
    height?: number;
}