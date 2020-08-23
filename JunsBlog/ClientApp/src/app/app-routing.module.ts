import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
    { path: 'home', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) },
    { path: 'article', loadChildren: () => import('./features/article/article.module').then(m => m.ArticleModule) },
    { path: 'login', loadChildren: () => import('./features/login/login.module').then(m => m.LoginModule) },
    { path: 'register', loadChildren: () => import('./features/register/register.module').then(m => m.RegisterModule) },
    { path: 'profile', loadChildren: () => import('./features/profile/profile.module').then(m => m.ProfileModule) },
    { path: 'social', loadChildren: () => import('./features/social/social.module').then(m => m.SocialModule) } 
  ];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled'
    })],
    exports: [RouterModule]
  })
  export class AppRoutingModule { }