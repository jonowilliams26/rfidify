<script lang="ts">
	import type { SpotifyImage, SpotifyItem } from '$lib/api/types/spotify';

	export let item: SpotifyItem;

	const image = () => {
		switch (item.type) {
			case 'Track':
				return getLastImage(item.album.images);
			default:
				return getLastImage(item.images);
		}
	};

	function getLastImage(images: SpotifyImage[]) {
		if (!images || images.length === 0) {
			return undefined;
		}
		return images[images.length - 1].url;
	}
</script>

<img src={image()} alt={item.name} class="h-16 w-16" />