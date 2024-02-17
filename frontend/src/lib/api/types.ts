export const SpotifyItemTypes = {
    track: 'track',
    album: 'album',
    artist: 'artist',
    playlist: 'playlist'
} as const;

const SpotifyItemTypeList = [SpotifyItemTypes.track, SpotifyItemTypes.album, SpotifyItemTypes.artist, SpotifyItemTypes.playlist] as const;
export type SpotifyItemType = typeof SpotifyItemTypeList[number];
export function isSpotifyItemType(type: string): type is SpotifyItemType {
    return SpotifyItemTypeList.includes(type as SpotifyItemType);
}

export type RFID = {
    id: string;
    spotifyItem: SpotifyItem;
}

export type SpotifyItem = Track | Artist | Album | Playlist;

export type Track = {
    type: typeof SpotifyItemTypes.track;
    id: string;
    name: string;
    uri: string;
    artists: Artist[];
    album: Album;
}

export type Artist = {
    type: typeof SpotifyItemTypes.artist;
    id: string;
    name: string;
    uri: string;
    images: Image[];
}

export type Album = {
    type: typeof SpotifyItemTypes.album;
    id: string;
    name: string;
    uri: string;
    images: Image[];
    artists: Artist[];
}

export type Playlist = {
    type: typeof SpotifyItemTypes.playlist;
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

export type PagedResponse<T> = {
    items: T[];
    next?: string;
    previous?: string;
    limit: number;
    offset: number;
    total: number;
}