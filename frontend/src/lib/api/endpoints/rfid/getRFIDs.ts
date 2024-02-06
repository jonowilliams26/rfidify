import { type FetchFn, getJson } from "../../fetch";
import type { RFIDTrack, RFIDAlbum, RFIDPlaylist, RFIDArtist } from "../../types/RFID";

type GetRFIDsResponse = GetRFIDsResponseItem[];
type GetRFIDsResponseItem = RFIDTrack | RFIDAlbum | RFIDPlaylist | RFIDArtist;

export default async function getRFIDs(fetch: FetchFn) {
    return await getJson<GetRFIDsResponse>(fetch, '/rfids');
}