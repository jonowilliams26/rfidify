<script lang="ts">
	import type { SpotifyItem } from '$lib/api/types';
	import { Link1, Person } from 'radix-icons-svelte';
	import { cn } from '$lib/utils';
	import { tv } from 'tailwind-variants';

	export let rfid: string | undefined = undefined;
	export let spotifyItem: SpotifyItem;
	export let size: 'default' | 'sm' = 'default';
	$: image = () => {
		const images = spotifyItem.type === 'Track' ? spotifyItem.album.images : spotifyItem.images;

		if (!images || images.length === 0) {
			return undefined;
		}

		return images[0].url;
	};

	const containerVariants = tv({
		base: 'flex truncate',
		variants: {
			size: {
				default: 'space-x-5',
				sm: 'space-x-2'
			}
		},
		defaultVariants: {
			size: 'default'
		}
	});

	const subTextVariants = tv({
		base: 'truncate',
		variants: {
			size: {
				default: 'text-sm',
				sm: 'text-xs'
			}
		},
		defaultVariants: {
			size: 'default'
		}
	});

	const iconVariants = tv({
		variants: {
			size: {
				default: 'h-5 w-5',
				sm: 'h-4 w-4'
			}
		},
		defaultVariants: {
			size: 'default'
		}
	});

	const imageVariants = tv({
		base: 'rounded-lg',
		variants: {
			size: {
				default: 'h-24 w-24',
				sm: 'h-14 w-14'
			}
		},
		defaultVariants: {
			size: 'default'
		}
	});

	const nameVariants = tv({
		base: 'truncate ',
		variants: {
			size: {
				default: 'text-lg font-bold',
				sm: 'text-sm font-semibold'
			}
		},
		defaultVariants: {
			size: 'default'
		}
	});

	function iconClass() {
		return cn(iconVariants({ size }));
	}

	function subTextClass() {
		return cn(subTextVariants({ size }));
	}
</script>

<div class={cn(containerVariants({ size }))}>
	<img src={image()} alt={spotifyItem.name} class={cn(imageVariants({ size }))} />
	<div class="flex flex-col space-y-1 truncate">
		<p class={cn(nameVariants({ size }))}>{spotifyItem.name}</p>
		<div class="text-muted-foreground space-y-1 truncate">
			{#if spotifyItem.type === 'Track' || spotifyItem.type === 'Album'}
				<div class="flex items-center space-x-1 truncate">
					<Person class={iconClass()} />
					<span class={subTextClass()}>
						{spotifyItem.artists.map((x) => x.name).join(', ')}
					</span>
				</div>
			{/if}
			{#if spotifyItem.type === 'Playlist'}
				<p class={subTextClass()}>{spotifyItem.description}</p>
			{/if}
			{#if rfid}
				<div class="flex items-center space-x-1 truncate">
					<Link1 class={iconClass()} />
					<span class={subTextClass()}>{rfid}</span>
				</div>
			{/if}
		</div>
	</div>
</div>
