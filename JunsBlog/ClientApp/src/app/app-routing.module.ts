import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) }, 
  { path: 'home', loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule) }, 
  { path: 'login', loadChildren: () => import('./features/login/login.module').then(m => m.LoginModule) }, 
  { path: 'register', loadChildren: () => import('./features/register/register.module').then(m => m.RegisterModule) },
  { path: 'profile/:id', loadChildren: () => import('./features/profile/profile.module').then(m => m.ProfileModule) },
  { path: 'editor', loadChildren: () => import('./features/article-editor/article-editor.module').then(m => m.ArticleEditorModule) },
  { path: 'editor/:id', loadChildren: () => import('./features/article-editor/article-editor.module').then(m => m.ArticleEditorModule) },
  { path: 'article/:id', loadChildren: () => import('./features/article-viewer/article-viewer.module').then(m => m.ArticleViewerModule) },
  { path: 'reset', loadChildren: () => import('./features/reset/reset.module').then(m => m.ResetModule) },
  { path: 'social', loadChildren: () => import('./features/social/social.module').then(m => m.SocialModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
