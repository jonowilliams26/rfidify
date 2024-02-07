export type SpotifyItem = SpotifyTrack | SpotifyArtist | SpotifyAlbum | SpotifyPlaylist;

type SpotifyId = {
    id: string;
    uri: string;
    name: string;
}

type SpotifyTrack = {
    type: "Track";
    artists: SpotifyArtist[];
    album: SpotifyAlbum;
} & SpotifyId;

type SpotifyArtist = {
    type: "Artist";
    images: SpotifyImage[];
} & SpotifyId;

type SpotifyAlbum = {
    type: "Album";
    images: SpotifyImage[];
    artists: SpotifyArtist[];
} & SpotifyId;

type SpotifyPlaylist = {
    type: "Playlist";
    description?: string;
    images: SpotifyImage[];
} & SpotifyId;

export type SpotifyImage = {
    url: string;
    height?: number;
    width?: number;
}