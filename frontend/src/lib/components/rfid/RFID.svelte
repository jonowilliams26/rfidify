<script lang="ts">
	import * as Card from '$lib/components/ui/card';
	import { Person, Link2, DotsHorizontal, Trash, Pencil1 } from 'radix-icons-svelte';
	import { Button } from '$lib/components/ui/button';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu';
	import type { RFID, SpotifyImage } from '$lib/api/types/RFID';

	export let rfid: RFID;
	export let showOptionsMenu = true;

    function getImage(){
        switch(rfid.spotifyItem.type){
            case 'Track':
                return getLastImage(rfid.spotifyItem.album.images);
            default:
                return getLastImage(rfid.spotifyItem.images);
        }
    }

    function getLastImage(images: SpotifyImage[]){
        if(!images || images.length === 0){
            return undefined;
        }
        return images[images.length - 1].url;
    }

</script>

<Card.Root>
	<Card.Content class="flex space-x-3 p-4 text-xs">
		<img src={getImage()} alt={rfid.spotifyItem.name} class="h-16 w-16" />
		<div class="flex w-full flex-col truncate">
			<div class="flex w-full items-start justify-between space-x-4 truncate pb-0.5">
				<h2 class="truncate text-sm font-semibold">{rfid.spotifyItem.name}</h2>
				{#if showOptionsMenu}
					<DropdownMenu.Root>
						<DropdownMenu.Trigger asChild let:builder>
							<Button builders={[builder]} variant="ghost" size="icon" class="h-6 w-6">
								<DotsHorizontal />
							</Button>
						</DropdownMenu.Trigger>
						<DropdownMenu.Content>
							<a href={`/rfid/${rfid.id}`}>
								<DropdownMenu.Item>
									<Pencil1 class="mr-1.5" />
									Edit
								</DropdownMenu.Item>
							</a>

							<DropdownMenu.Separator />
							<DropdownMenu.Item>
								<Trash class="mr-1.5" />
								Delete
							</DropdownMenu.Item>
						</DropdownMenu.Content>
					</DropdownMenu.Root>
				{/if}
			</div>
			<div class="space-y-1 truncate text-xs text-muted-foreground">
				{#if rfid.spotifyItem.type === 'Track' || rfid.spotifyItem.type === 'Album'}
					<div class="flex space-x-1">
						<Person />
						<span>{rfid.spotifyItem.artists.map(x => x.name)}</span>
					</div>
				{/if}
				{#if rfid.spotifyItem.type === 'Playlist'}
					<span class="truncate">{rfid.spotifyItem.description}</span>
				{/if}
				<div class="flex space-x-1">
					<Link2 />
					<span>{rfid.id}</span>
				</div>
			</div>
		</div>
	</Card.Content>
</Card.Root>
