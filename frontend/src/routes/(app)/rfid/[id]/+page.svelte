<script lang="ts">
    import type { PageData } from './$types';
    
    export let data: PageData;
    const { rfid } = data;

</script>

{#if !rfid}
    <p>RFID not found</p>
{:else}
    <p>RFID found</p>
{/if}