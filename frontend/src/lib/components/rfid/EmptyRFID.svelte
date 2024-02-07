<script lang="ts">
	import { Skeleton } from '$lib/components/ui/skeleton';
	import Card from './Card.svelte';
	import TagId from './TagId.svelte';

	export let id: string;
</script>

<Card>
	<Skeleton class="h-16 w-16" />
	<div class="flex flex-col space-y-2 text-xs text-muted-foreground">
		<Skeleton class="mb-0.5 h-4 w-96" />
		<Skeleton class="h-4 w-72" />
		<TagId rfid={id} />
	</div>
</Card>