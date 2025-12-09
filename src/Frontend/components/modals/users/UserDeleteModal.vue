<template>
    <Transition name="modal">
      <BaseModal v-if="showModal" :show-close="true" :showing="showModal" @close="close">
        <template #title>
          <span class="break-normal capitalize-block">
            {{ $t('delete') }}
          </span>
        </template>
        <template #body>
          <div class="my-4">
            <p class="">
              {{ $t('delete_confirmation') }}
            </p>
          </div>
        </template>
        <template #action>
          <button class="btn bg-error w-full text-white capitalize-block" @click="submit" :disabled="isLoading">
            {{ $t('delete') }}<i v-show="isLoading" class="fas fa-spinner fa-spin text-white ml-2"></i>
          </button>
        </template>
      </BaseModal>
    </Transition>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue'
  import { useUserStore } from '~/stores/UserStore'
  
  export default defineComponent({
    name: 'UserDeleteModal',
    emits: ['item-deleted'],
    setup(props, context) {
      const showModal = ref(false)
      const isLoading = ref(false)
      const selectedIds = ref([] as string[])
  
      const open = (data: any) => {
        showModal.value = true
        selectedIds.value = data
      }
  
      const close = () => {
        selectedIds.value = []
        showModal.value = false
        isLoading.value = false
      }
  
      const { deleteUser } = useUserStore()
      const submit = async (e: MouseEvent) => {
        isLoading.value = true
        const deletePromises = selectedIds.value.map(async id => {
          await deleteUser(id);
        });
        await Promise.all(deletePromises);
        isLoading.value = false
        context.emit('item-deleted')
        close()
      }
  
      return {
        selectedIds,
        showModal,
        open,
        close,
        isLoading,
        submit
      }
    },
  })
  </script>
  