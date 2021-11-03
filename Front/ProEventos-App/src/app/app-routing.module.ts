import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContatoComponent } from './components/contato/contato.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EventosDetalheComponent } from './components/eventos/eventos-detalhe/eventos-detalhe.component';
import { EventosListComponent } from './components/eventos/eventos-list/eventos-list.component';
import { EventosComponent } from './components/eventos/eventos.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user.component';

const routes: Routes = [
  {
    path: 'user', component: UserComponent,
    children : [
      { path: 'login', component: LoginComponent },
      { path: 'registrar', component: RegistrationComponent }
    ]
  },
  { path: 'user/perfil', component: PerfilComponent },
  { path:'eventos', redirectTo:'eventos/listar', pathMatch: 'full' },
  {
    path: 'eventos', component: EventosComponent,
    children : [
      { path: 'detalhe/:id', component: EventosDetalheComponent },
      { path: 'detalhe', component: EventosDetalheComponent },
      { path: 'listar', component: EventosListComponent }
    ]
  },
  { path: 'palestrantes', component: PalestrantesComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'contato', component: ContatoComponent },
  { path:'', redirectTo:'dashboard', pathMatch: 'full' },
  { path:'**', redirectTo:'dashboard', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
