<script lang="ts">
	import { Skeleton } from '$lib/components/ui/skeleton';
	import { Link2, Person } from 'radix-icons-svelte';

	export let rfid: string;
</script>

<div class="flex space-x-5">
	<Skeleton class="h-24 w-24 rounded-lg" />
	<div class="flex flex-col space-y-3">
		<Skeleton class="h-6 w-96 " />
		<div class="text-muted-foreground space-y-1">
			<Skeleton class="h-6 w-64" />
			<div class="flex items-center space-x-1">
				<Link2 class="h-5 w-5" />
				<span>{rfid}</span>
			</div>
		</div>
	</div>
</div>