import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {

  public urlBase = environment.apiURL + 'api/redesSociais/';

  constructor(private http: HttpClient) { }

  public getRedesSociais(origem: string, id: number): Observable<RedeSocial[]>
  {
    let url = id === 0 ? `${this.urlBase}${origem}` : `${this.urlBase}${origem}/${id}`;

    return this.http.get<RedeSocial[]>(url).pipe(take(1));
  }

  public salverRedeSocial(origem: string, id: number, redeSocial: RedeSocial[]): Observable<RedeSocial[]>
  {
    let url = id === 0 ? `${this.urlBase}${origem}` : `${this.urlBase}${origem}/${id}`;

    return this.http.put<RedeSocial[]>(url, redeSocial).pipe(take(1));
  }

  public delete(origem: string, id: number, rdSocialId: number): Observable<any>
  {
    let url = id === 0 ? `${this.urlBase}${origem}/${rdSocialId}` : `${this.urlBase}${origem}/${id}/${rdSocialId}`;

    return this.http.delete<string>(url).pipe(take(1));
  }
}
