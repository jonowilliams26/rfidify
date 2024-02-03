<script lang="ts">
    import type { PageData } from './$types';
    import { Button } from "$lib/components/ui/button";
    
    export let data: PageData;
</script>

<h1>RFIDs</h1>
<a href="/">Go Home</a>
<Button>Click me</Button>

{#each data.rfids as rfid}
    <p class="text-xl font-bold">{rfid.rfid}</p>
    <p>{rfid.type}</p>
    <p>{rfid.name}</p>
{/each}
   