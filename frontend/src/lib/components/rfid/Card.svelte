<script lang="ts">
	import * as Card from '$lib/components/ui/card';
</script>

<Card.Root>
	<Card.Content class="flex space-x-3 p-4 text-xs">
		<slot />
	</Card.Content>
</Card.Root>
