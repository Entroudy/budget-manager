<template>
  <main class="auth-shell min-h-screen bg-[#f6f3e9] lg:grid lg:grid-cols-[minmax(360px,0.72fr)_minmax(0,1.28fr)]">
    <section class="relative z-10 flex min-h-screen flex-col bg-[#17382d] px-7 py-7 text-[#f6f3e9] shadow-2xl sm:px-12 lg:px-14 xl:px-20">
      <div class="flex items-center justify-between">
        <NuxtLink to="/login" class="flex items-center gap-3">
          <img src="~/assets/images/budget-manager-logo.png" alt="Budget Manager" class="h-12 w-12 rounded-2xl object-cover shadow" />
          <span class="font-display text-xl font-semibold">Budget Manager</span>
        </NuxtLink>
        <LanguageSwitcher variant="header" />
      </div>

      <div class="my-auto mx-auto w-full max-w-md py-16 lg:py-20">
        <div class="mb-9">
          <p class="mb-3 text-xs font-bold uppercase tracking-[.22em] text-[#aec0a8]">{{ $t('welcome_back') }}</p>
          <h1 class="font-display text-4xl font-semibold leading-tight text-[#fffdf6] sm:text-5xl">{{ $t('sign_in') }}</h1>
          <p class="mt-4 max-w-sm text-sm leading-6 text-[#cfdbca]">{{ $t('sign_in_to_manage_finances') }}</p>
        </div>
        <div class="rounded-[1.75rem] border border-white/10 bg-white/[.06] p-5 shadow-xl backdrop-blur sm:p-7">
          <slot />
        </div>
        <p class="mt-7 text-xs text-[#a9bba5]">{{ $t('mindful_money') }}</p>
      </div>
      <div class="flex items-center gap-2 text-xs text-[#a9bba5]"><i class="fas fa-sprout" /><span>Budget Manager</span><span class="text-white/30">·</span><span>{{ $t('personal_finance') }}</span></div>
    </section>

    <section class="auth-illustration relative hidden min-h-screen overflow-hidden p-8 lg:flex lg:items-center lg:justify-center xl:p-14">
      <div class="absolute inset-0 bg-[#f6f3e9]" />
      <div class="auth-card relative flex min-h-[80vh] w-full max-w-6xl flex-col items-center justify-center overflow-hidden rounded-[2.5rem] bg-[#6f9476] px-10 py-16 text-center text-[#fffdf6] shadow-2xl xl:px-20">
        <div class="auth-sun absolute right-[13%] top-[12%] h-44 w-44 rounded-full bg-[#e6c989]/80 blur-[1px]" />
        <div class="auth-hill auth-hill-one absolute bottom-0 left-[-8%] h-[35%] w-[75%] rounded-[50%] bg-[#456c54]" />
        <div class="auth-hill auth-hill-two absolute bottom-[-5%] right-[-12%] h-[42%] w-[78%] rounded-[50%] bg-[#365c47]" />
        <div class="auth-plant absolute bottom-[17%] left-[18%]"><PoppyMark class="h-16 w-16" /></div>
        <div class="auth-plant absolute bottom-[27%] right-[19%] rotate-[-28deg]"><PoppyMark class="h-12 w-12" /></div>
        <div class="relative z-10 max-w-3xl pb-16">
          <div class="mx-auto mb-8 flex h-[240px] w-[240px] items-center justify-center rounded-[2.5rem] border border-white/30 bg-white/15 p-3 shadow-lg backdrop-blur"><img src="~/assets/images/budget-manager-logo.png" alt="Budget Manager" class="h-full w-full object-contain" /></div>
          <p class="mb-4 text-xs font-bold uppercase tracking-[.25em] text-[#e6e9d6]">{{ $t('your_money_garden') }}</p>
          <h2 class="font-display text-4xl font-semibold leading-tight sm:text-5xl xl:text-6xl">{{ $t('small_steps') }}</h2>
          <p class="mx-auto mt-5 max-w-xl text-base leading-7 text-white/85">{{ $t('mindful_money') }}</p>
          <div class="mt-12 flex flex-wrap justify-center gap-3">
            <div class="rounded-2xl border border-white/15 bg-[#214638]/25 px-5 py-4 backdrop-blur"><i class="fas fa-wallet mb-2 block text-lg text-[#f0d89a]" /><span class="text-sm">{{ $t('spendings') }}</span></div>
            <div class="rounded-2xl border border-white/15 bg-[#214638]/25 px-5 py-4 backdrop-blur"><i class="fas fa-chart-pie mb-2 block text-lg text-[#f0d89a]" /><span class="text-sm">{{ $t('income') }}</span></div>
            <div class="rounded-2xl border border-white/15 bg-[#214638]/25 px-5 py-4 backdrop-blur"><PoppyMark class="mx-auto mb-2 h-7 w-7" expand /><span class="text-sm">{{ $t('remaining') }}</span></div>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  data() { return { notificationHandler: null as any } },
  mounted() {
    this.notificationHandler = (data) => this.$showNotification(data.type, data.message)
    this.$listen('notification', this.notificationHandler)
  },
  beforeUnmount() {
    if (this.notificationHandler) this.$unlisten('notification', this.notificationHandler)
  }
})
</script>
