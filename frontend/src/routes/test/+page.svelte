<script lang="ts">
    import type { PageData } from './$types';
    
    export let data: PageData;
</script>

<h1>RFIDs</h1>
<a href="/">Go Home</a>

{#each data.rfids as rfid}
    <p>{rfid.rfid}</p>
    <p>{rfid.type}</p>
    <p>{rfid.name}</p>
{/each}
   