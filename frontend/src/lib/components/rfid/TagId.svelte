<script lang="ts">
    import { Link2 } from 'radix-icons-svelte';
	export let rfid: string;
</script>

<div class="flex space-x-1">
    <Link2 />
    <span>{rfid}</span>
</div>
