<script lang="ts">
	import type { PageData } from './$types';
	import { RFID } from '$lib/components/rfid';

	export let data: PageData;
</script>

<div class="grid grid-cols-1 gap-2 md:grid-cols-2 xl:grid-cols-3">
	{#each data.rfids as rfid}
		<RFID rfid={rfid} />
	{/each}
</div>
