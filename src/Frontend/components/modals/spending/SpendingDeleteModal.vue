<template>
  <Transition name="modal">
    <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close" :preventBackgroundClose="true">
      <template #title>
        <span class="break-normal">
          {{ $t('delete') }}
        </span>
      </template>
      <template #body>
        <div class="text-center py-4">
          <i class="fas fa-exclamation-triangle text-yellow-500 text-6xl mb-4"></i>
          <p class="text-lg mb-2">{{ $t('are_you_sure') }}</p>
          <p class="text-sm text-gray-600 dark:text-gray-400">
            {{ $t('delete_spending_confirmation') }}
          </p>
        </div>
      </template>
      <template #action>
        <div class="flex space-x-4">
          <button class="btn bg-gray-500 w-full text-white capitalize" @click="close">
            {{ $t('cancel') }}
          </button>
          <button class="btn bg-red-500 w-full text-white capitalize" @click="submit" :disabled="isLoading">
            {{ $t('delete') }}<i v-show="isLoading" class="fas fa-spinner fa-spin text-white ml-2"></i>
          </button>
        </div>
      </template>
    </BaseModal>
  </Transition>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { useSpendingStore } from '~/stores/SpendingStore'

export default defineComponent({
  name: 'SpendingDeleteModal',
  emits: ['item-deleted'],
  setup(props, context) {
    const { t } = useI18n()
    const isLoading = ref(false)
    const showModal = ref(false)
    const selectedIds = ref<number[]>([])

    const { deleteSpending } = useSpendingStore()
    const { $showNotification } = useNuxtApp()

    const open = (ids: number[]) => {
      selectedIds.value = ids
      showModal.value = true
    }

    const close = () => {
      showModal.value = false
      selectedIds.value = []
    }

    const submit = async () => {
      isLoading.value = true
      try {
        let successCount = 0
        for (const id of selectedIds.value) {
          const response = await deleteSpending(id)
          if (response !== null) {
            successCount++
          }
        }

        if (successCount > 0) {
          $showNotification('success', t('spending_deleted_successfully', { count: successCount }))
          context.emit('item-deleted')
          close()
        }
      } catch (error) {
        console.error('Error deleting spending:', error)
      } finally {
        isLoading.value = false
      }
    }

    return {
      isLoading,
      showModal,
      selectedIds,
      open,
      close,
      submit,
    }
  },
})
</script>
