<script lang="ts">
	import type { RFID } from '$lib/api/types';
	import { Link2, Person } from 'radix-icons-svelte';

	export let rfid: RFID;

	$: image = () => {
		const images = rfid.spotifyItem.type === 'Track' 
            ? rfid.spotifyItem.album.images 
            : rfid.spotifyItem.images;

		if (!images || images.length === 0) {
            return undefined;
        }

		return images[0].url;
	};
</script>

<div class="flex space-x-5">
	<img src={image()} alt="" class="h-24 w-24 rounded-lg" />
	<div class="flex flex-col space-y-2">
		<h2 class="text-2xl font-bold">{rfid.spotifyItem.name}</h2>
		<div class="text-muted-foreground space-y-1">
			{#if rfid.spotifyItem.type === 'Track' || rfid.spotifyItem.type === 'Album'}
				<div class="flex items-center space-x-1">
					<Person class="h-5 w-5" />
					<span>{rfid.spotifyItem.artists.map((x) => x.name)}</span>
				</div>
			{/if}
            {#if rfid.spotifyItem.type === 'Playlist'}
                <p>{rfid.spotifyItem.description}</p>
            {/if}
			<div class="flex items-center space-x-1">
				<Link2 class="h-5 w-5" />
				<span>{rfid.id}</span>
			</div>
		</div>
	</div>
</div>
