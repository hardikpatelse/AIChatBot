import { Routes } from '@angular/router'

export const routes: Routes = [
    {
        path: 'chat',
        loadChildren: () =>
            import('./components/chat/chat.module').then((m) => m.ChatModule),
    },
    { path: '', redirectTo: 'chat', pathMatch: 'full' },
    { path: '**', redirectTo: 'chat' },
]
