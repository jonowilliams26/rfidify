<script lang="ts">
	import type { PageData } from './$types';
	import { Button } from '$lib/components/ui/button';
	import { Card } from '$lib/components/ui/card';
	import { DotsHorizontal, Pencil1, Trash } from 'radix-icons-svelte';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu';
	import { deleteRFID } from '$lib/api/endpoints';
	import { toast } from 'svelte-sonner';
	import { SpotifyItem } from '$lib/components/spotify';

	export let data: PageData;

	async function onDeleteClick(id: string) {
		const response = await deleteRFID(fetch, id);
		if (!response.ok) {
			toast.error('Sorry, something went wrong. Please try again.');
			return;
		}

		data.rfids = data.rfids.filter((x) => x.id !== id);
	}
</script>

{#if data.rfids.length === 0}
	<div class="mx-auto flex max-w-sm flex-col items-center justify-center space-y-4">
		<img src="home-empty-state.svg" alt="empty-state" />
		<Button class="w-full" href="/rfid/setup">Setup your first RFID</Button>
	</div>
{:else}
	<div class="grid grid-cols-1 gap-3 md:grid-cols-2 lg:grid-cols-3">
		{#each data.rfids as rfid}
			<Card class="flex items-start justify-between p-2.5">
				<SpotifyItem rfid={rfid.id} spotifyItem={rfid.spotifyItem}/>
				<DropdownMenu.Root>
					<DropdownMenu.Trigger asChild let:builder>
						<Button builders={[builder]} variant="ghost" size="icon" class="flex-shrink-0">
							<DotsHorizontal />
						</Button>
					</DropdownMenu.Trigger>
					<DropdownMenu.Content>
						<DropdownMenu.Item class="flex items-center space-x-1.5" href={`/rfid/${rfid.id}`}>
							<Pencil1 />
							<span>Edit</span>
						</DropdownMenu.Item>
						<DropdownMenu.Item
							class="flex items-center space-x-1.5 "
							on:click={() => onDeleteClick(rfid.id)}
						>
							<Trash />
							<span>Delete</span>
						</DropdownMenu.Item>
					</DropdownMenu.Content>
				</DropdownMenu.Root>
			</Card>
		{/each}
	</div>
{/if}
