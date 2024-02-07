import { type FetchFn, getJson } from "../../fetch";
import type { RFID } from "../../types/rfid";

export default async function getRFIDById(fetch: FetchFn, id: string) {
    return await getJson<RFID>(fetch, `/rfids/${id}`);
}