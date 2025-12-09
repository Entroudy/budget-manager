<template>
  <div></div>
</template>

<script setup lang="ts">
definePageMeta({
  middleware: 'auth',
})

// Redirect to spendings page
navigateTo('/spendings')
</script>
