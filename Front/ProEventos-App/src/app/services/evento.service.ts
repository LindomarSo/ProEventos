import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { map, take } from 'rxjs/operators'
import { environment } from 'src/environments/environment';
import { PaginationResult } from '@app/models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  baseURL = environment.apiURL+"api/evento";

  constructor(private http: HttpClient) { }

  public getAll(page?: number, itemsPerPage?: number, term?: string): Observable<PaginationResult<Evento[]>>
  {
    const paginationResult: PaginationResult<Evento[]> = new PaginationResult<Evento[]>();

    let params =  new HttpParams();

    if(page != null && itemsPerPage != null)
    {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if(term != null && term != '')
      params = params.append('term', term);

    return this.http.get<Evento[]>(this.baseURL, {observe: 'response', params})
            .pipe(take(1), map((response: any) => {

              paginationResult.result = response.body; // O body da requisição é mapeado para um tipo

              if(response.headers.has('Pagination')) // Se tiver alguma paginação
              {
                paginationResult.pagination = JSON.parse(response.headers.get('Pagination')); // Aqui mapeamos para o objeto pagination
              }
              return paginationResult;
            }));
  }

  public getAllByTema(tema: string): Observable<Evento[]>
  {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${tema}`)
            .pipe(take(1));
  }

  public getById(id: number): Observable<Evento>
  {
    return this.http.get<Evento>(`${this.baseURL}/${id}`)
              .pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento>
  {
    return this.http.post<Evento>(this.baseURL, evento);
  }

  public put(evento: Evento): Observable<Evento>
  {
    return this.http.put<Evento>(`${this.baseURL}/${evento.id}`, evento)
        .pipe(take(1));
  }

  public delete(id: number): Observable<any>
  {
    return this.http.delete(`${this.baseURL}/${id}`)
          .pipe(take(1));
  }

  public postUpload(eventoId: number, file: File[]): Observable<Evento>
  {
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http.post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData)
            .pipe(take(1));
  }
}
