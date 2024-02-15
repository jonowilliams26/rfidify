<script lang="ts">
	import type { PageData } from './$types';
    import { Button } from "$lib/components/ui/button";

	export let data: PageData;
</script>

{#if data.rfids.length === 0}
	<div class="flex flex-col items-center justify-center max-w-sm mx-auto space-y-4">
		<img src="home-empty-state.svg" alt="empty-state"/>
        <Button class="w-full" href="/rfid/setup">Setup your first RFID</Button>
	</div>
{:else}
	<p>RFIDs found</p>
{/if}
