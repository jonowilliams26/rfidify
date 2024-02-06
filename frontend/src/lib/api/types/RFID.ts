export type RFID = {
    id: string;
    spotifyItem: SpotifyTrack | SpotifyArtist | SpotifyAlbum | SpotifyPlaylist;
}

type SpotifyItem = {
    id: string;
    uri: string;
    name: string;
};

type SpotifyTrack = {
    type: "Track";
    artists: SpotifyArtist[];
    album: SpotifyAlbum;
} & SpotifyItem;

type SpotifyArtist = {
    type: "Artist";
    images: SpotifyImage[];
} & SpotifyItem;

type SpotifyAlbum = {
    type: "Album";
    images: SpotifyImage[];
    artists: SpotifyArtist[];
} & SpotifyItem;

type SpotifyPlaylist = { 
    type: "Playlist";
    description?: string;
    images: SpotifyImage[];
} & SpotifyItem;

export type SpotifyImage = {
    url: string;
    height?: number;
    width?: number;
}
