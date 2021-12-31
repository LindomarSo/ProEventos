import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginationResult } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteUpdate } from '@app/models/PalestranteUpdate';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {

  public urlBase = environment.apiURL + 'api/palestrante/';

  constructor(private http: HttpClient) { }

  public getAll(page?: number, itemsPerPage?: number, term?: string): Observable<PaginationResult<Palestrante[]>>
  {
    const paginationResult: PaginationResult<Palestrante[]> = new PaginationResult();

    let params = new HttpParams();

    if(page != null && itemsPerPage != null)
    {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if(term != null && term != '')
    {
      params = params.append('term', term);
    }

    return this.http.get<Palestrante[]>(this.urlBase + 'all', {observe:'response', params}).pipe(take(1), map((response: any) => {
      paginationResult.result = response.body;

      if(response.headers.has('Pagination'))
      {
        paginationResult.pagination = response.headers.get('Pagination');
      }

      return paginationResult;
    }));
  }

  public getByUserId(): Observable<Palestrante>
  {
    return this.http.get<Palestrante>(this.urlBase).pipe(take(1));
  }

  public post(): Observable<Palestrante>
  {
    return this.http.post<Palestrante>(this.urlBase, {} as Palestrante).pipe(take(1));
  }

  public put(model: Palestrante): Observable<Palestrante>
  {
    return this.http.put<Palestrante>(this.urlBase, model).pipe(take(1));
  }
}
