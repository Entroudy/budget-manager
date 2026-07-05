<template>
  <div ref="sidebar" id="sidebar" :class="[sidebarOpen ? '!-left-262px' : '']"
    class="fixed xl:w-64 transition-all z-50 h-screen xl:fixed xl:inset-y-0 xl:left-0 xl:z-50 xl:flex xl:flex-col justify-between xl:overflow-y-auto bg-brand-darkGray xl:pb-4 print:hidden">
    <div>
      <div class="flex shrink-0 items-center mt-4 mb-2 transition-all justify-center">
        <img class="w-auto h-20" src="~/public/images/rts-logo.svg" />
      </div>
      <nav>
        <ul role="list" class="flex flex-col space-y-1 lg:space-y-2 py-4 px-4 lg:py-4 lg:px-4"
          :class="[sidebarOpen ? 'sidebarClosed' : 'sidebarOpen']">
          <template v-for="item in menuItems.top">
            <li :class="[sidebarOpen ? 'flex justify-center' : '']">
              <template v-if="!item.children">
                <NuxtLink :to="item.link" @click="menuClicked"
                  class="text-white group flex gap-x-3 rounded-md text-sm leading-6 font-semibold items-center"
                  :class="[sidebarOpen ? 'lg:justify-center p-4' : 'px-4 py-3', !sidebarOpen ? 'hover:bg-brand-darkGray/20' : '']">
                  <i class="text-md" :class="item.iconClass" />
                  <span class="menu-item-text capitalize-inline" :class="[sidebarOpen ? 'hidden' : '']">{{ $t(item.name.toLowerCase())
                    }}</span>
                </NuxtLink>
              </template>
              <template v-if="item.children">
                <button @click="handleSubmenuClick(item.name)"
                  class="text-white group flex justify-between gap-x-3 rounded-md text-sm leading-6 font-semibold items-center"
                  :class="[sidebarOpen ? 'lg:justify-center p-4' : 'px-4 py-3', item.active && sidebarOpen ? 'active' : 'inactive', !sidebarOpen ? 'w-full hover:bg-brand-darkGray/20' : '']">
                  <span>
                    <i class="text-md"
                      :class="item.iconClass, !sidebarOpen ? 'mr-3.5' : '', item.active && !sidebarOpen ? 'text-white font-bold' : '', item.active && sidebarOpen ? 'text-primary' : ''" />
                    <span class="menu-item-text capitalize-inline inline-block"
                      :class="[sidebarOpen ? 'hidden' : '', item.active ? 'text-white' : '']">{{ $t(item.name) }}</span>
                  </span>
                  <i v-show="!sidebarOpen" class="ml-2.5 text-xs fas"
                    :class="[item.active ? 'fa-chevron-up' : 'fa-chevron-down']" />
                </button>
                <div :class="[item.active && !sidebarOpen ? 'lg:block' : 'hidden']">
                  <ul class="subitem-list space-y-2">
                    <template v-for="(subItem, index) in item.children">
                      <li>
                        <NuxtLink :to="subItem.link" @click="menuClicked"
                          :class="[isMenuItemActive(subItem) ? 'router-link-exact-active' : '']"
                          class="hover:bg-brand-darkGray/20 hover:text-white-60 flex rounded-md text-white/80 text-sm leading-6 font-semibold items-center pl-5 pr-5 py-2">
                          <i class="far fa-circle text-xs"></i>
                          <span class="pl-3 capitalize-inline">
                            {{ $t(subItem.label) }}
                          </span>
                        </NuxtLink>
                      </li>
                    </template>
                  </ul>
                </div>
              </template>
            </li>
          </template>
        </ul>
      </nav>
      <nav>
      <ul role="list" class="flex flex-col space-y-2 lg:space-y-4 px-4 lg:px-6">
        <template v-for="item in menuItems.bottom">
          <li v-if="item.name !== 'Logout'" :key="item.name">
            <NuxtLink :to="item.link"
              class="hover:text-white text-gray-400 group flex gap-x-3 rounded-md text-sm leading-6 font-semibold items-center"
              :class="[sidebarOpen ? 'lg:justify-center px-5 lg:px-3 py-4' : 'px-5 py-3']">
              <i :class="item.iconClass" />
              <span :class="[sidebarOpen ? 'lg:sr-only' : '']">{{ item.name }}</span>
            </NuxtLink>
          </li>
        </template>
        <li
          class="cursor-pointer text-gray-400  group flex gap-x-3 rounded-md text-sm leading-6 font-semibold items-center hover:text-white"
          :class="[sidebarOpen ? 'lg:justify-center px-5 lg:px-3 py-4' : 'px-5 py-3']" @click="logout">
          <i class="fas fa-sign-out-alt" />
          <span :class="[sidebarOpen ? 'lg:sr-only' : '']">{{ $t('logout') }}</span>
        </li>
      </ul>
    </nav>
    </div>
    <div class="flex justify-center text-gray-400 text-sm pb-4"
      :class="[sidebarOpen ? 'absolute bottom-4 left-1/2 -translate-x-1/2' : '']">
      <span class="inline-flex text-center text-xs"><span :class="[sidebarOpen ? '!hidden' : '']"
          class="lg:flex">service-manager -</span> <span>v2.0.1</span></span>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { mapActions, mapState } from 'pinia'
import { TopMenu, BottomMenu } from '@/core/menuItems'

export default defineComponent({
  name: 'TheSidebar',
  props: {
    sidebarOpen: {
      type: Boolean,
      required: true
    }
  },
  emits: ['on-close-menu', 'on-open-sidebar'],
  data() {
    return {
      showProfileDropdown: false,
      menuItems: {
        top: TopMenu,
        bottom: BottomMenu
      }
    }
  },
  computed: {
    sidebarLogoPath() {
      return this.sidebarOpen ? '/images/rts-logo.svg' : '/images/rts-logo.svg'
    }
  },
  watch: {
    '$route.path': function (newPath) {
      this.menuItems.top.forEach(item => {
        item.active = false;
      });
      if (newPath) this.setMenuItemActiveState(newPath)
    }
  },
  mounted() {
    this.setMenuItemActiveState(this.$route.path)
  },
  methods: {
    isMenuItemActive(path: any) {
      return this.$route.path.startsWith(path.link);
    },
    handleDropdownClick() {
      if (this.showProfileDropdown) {
        this.showProfileDropdown = false;
      }
    },
    setMenuItemActiveState(routePath: string) {
      const currentPath = routePath.split('/').map(segment => '-' + segment).join('');

      this.menuItems.top.forEach(item => {
        const itemPath = item.link.split('/').map(segment => '-' + segment).join('')
        if (currentPath.includes(itemPath) && itemPath !== '--') {
          item.active = true
          return
        }
        if (currentPath == '--' && itemPath == '--') {
          item.active = true
          return
        }
      });
    },
    handleSubmenuClick(itemName: string) {
      this.menuItems.top = this.menuItems.top.map(item => {
        if (item.name === itemName) {
          const isActive = !item.active
          return { ...item, active: isActive };
        } else if (item.children) {
          const updatedChildren = item.children.map(child => {
            if (child.label === itemName) {
              return { ...child, active: true };
            } else {
              return child;
            }
          });
          return { ...item, children: updatedChildren };
        } else {
          return item;
        }
      });
    },
    menuClicked() {
      if (window.innerWidth < 1280) {
        this.$emit('on-open-sidebar')
      }
    },
    ...mapActions(useAuthStore, ['logout']),
  }
})
</script>

<style>
#sidebar {
  transition: width 0.25s ease, left 0.25s ease;
  overflow-x: hidden;
}

.menu-item-text {
  transition: display 5s ease;
}
</style>
