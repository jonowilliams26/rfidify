<script lang="ts">
	import * as Card from '$lib/components/ui/card';
	import { Person, Link2, DotsHorizontal, Trash, Pencil1 } from 'radix-icons-svelte';
	import { Button } from '$lib/components/ui/button';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu';
    import type { RFIDTrack, RFIDAlbum, RFIDArtist, RFIDPlaylist } from '$lib/api/types/RFID';

    export let rfid: RFIDTrack | RFIDAlbum | RFIDArtist | RFIDPlaylist;
    export let showOptionsMenu = true;

</script>

<Card.Root>
	<Card.Content class="flex space-x-3 p-4 text-xs">
		<img src={rfid.image} alt={rfid.name} class="h-16 w-16" />
		<div class="flex w-full flex-col truncate">
			<div class="flex w-full items-start justify-between space-x-4 truncate pb-0.5">
				<h2 class="truncate text-sm font-semibold">{rfid.name}</h2>
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
				{#if rfid.type === 'Track' || rfid.type === 'Album'}
					<div class="flex space-x-1">
						<Person />
						<span>{rfid.artists}</span>
					</div>
				{/if}
				{#if rfid.type === 'Playlist'}
					<span class="truncate">{rfid.description}</span>
				{/if}
				<div class="flex space-x-1">
					<Link2 />
					<span>{rfid.id}</span>
				</div>
			</div>
		</div>
	</Card.Content>
</Card.Root>
