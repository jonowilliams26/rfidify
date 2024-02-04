import { toast } from "svelte-sonner";

export function toastError() {
    toast.error("Sorry, something went wrong,", {
        description: "Please try again later."
    })
}