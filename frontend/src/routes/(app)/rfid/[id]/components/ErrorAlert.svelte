<script lang="ts">
	import * as Alert from "$lib/components/ui/alert";
	import { ExclamationTriangle } from "radix-icons-svelte";
</script>

<Alert.Root variant="destructive">
    <ExclamationTriangle class="h-4 w-4" />
    <Alert.Title>Sorry, something went wrong</Alert.Title>
    <Alert.Description>
        An unexpected error occurred trying to load data from Spotify. Please try again.
    </Alert.Description>
</Alert.Root>