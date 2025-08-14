import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { Onboard } from './components/onboard/onboard';
import { Home } from './components/home/home';
import { Dashboard } from './components/dashboard/dashboard';
import { Homedash } from './components/homedash/homedash';
import { Friendsdash } from './components/friendsdash/friendsdash';
import { Notificationsdash } from './components/notificationsdash/notificationsdash';
import { Profile } from './components/profile/profile';

export const routes: Routes = [
    {path: "", component: Home, children: [
        {path: '', redirectTo: 'login', pathMatch: 'full'},
        {path: "login", component: Login},
        {path: "register", component: Register}
    ]},
    {path: "onboard", component: Onboard},
    {path: "dashboard", component: Dashboard, children: [
        {path: '', component: Homedash},
        {path: 'friends', component: Friendsdash},
        {path: 'notifications', component: Notificationsdash},
        {path: "profile", component: Profile}
    ]}
];