import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/auth.guard';

const routes: Routes = [
    { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
    { path: 'home', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
    { path: 'article/:id', loadChildren: () => import('./features/article/article.module').then(m => m.ArticleModule) },
    { path: 'login', loadChildren: () => import('./features/login/login.module').then(m => m.LoginModule) },
    { path: 'register', loadChildren: () => import('./features/register/register.module').then(m => m.RegisterModule) },
    { path: 'profile/:id', loadChildren: () => import('./features/profile/profile.module').then(m => m.ProfileModule), canActivate: [AuthGuard] },
    { path: 'social', loadChildren: () => import('./features/social/social.module').then(m => m.SocialModule) },
    { path: 'editor/:id', loadChildren: () => import('./features/editor/editor.module').then(m => m.EditorModule), canActivate: [AuthGuard] },
    { path: 'editor', loadChildren: () => import('./features/editor/editor.module').then(m => m.EditorModule), canActivate: [AuthGuard] },
    { path: 'search', loadChildren: () => import('./features/search/search.module').then(m => m.SearchModule) },
    { path: 'reset', loadChildren: () => import('./features/reset/reset.module').then(m => m.ResetModule) } 
  ];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled'
    })],
    exports: [RouterModule]
  })
  export class AppRoutingModule { }